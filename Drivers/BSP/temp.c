#include "temp.h"
#include "iic.h"
#include "delay.h"

#define TEMP_CLK_ENABLE __HAL_RCC_GPIOB_CLK_ENABLE()

iic_bus_t temp_pins = 
{
	.IIC_SDA_PORT = GPIOB,
	.IIC_SCL_PORT = GPIOB,
	.IIC_SDA_PIN  = GPIO_PIN_13,
	.IIC_SCL_PIN  = GPIO_PIN_14,
};

uint8_t temp_read_status(void)
{
	uint8_t Byte_first;	
	IICStart(&temp_pins);
	IICSendByte(&temp_pins,0x71);
	IICWaitAck(&temp_pins);
	Byte_first = IICReceiveByte(&temp_pins);
	IICSendNotAck(&temp_pins);
	IICStop(&temp_pins);	
	return Byte_first;
}

uint8_t temp_read_cal_enable(void)  
{
	uint8_t val = 0;
 
  val = temp_read_status();
  if((val & 0x68)==0x08) 
		return 1;
  else  
		return 0;
}

void temp_reset(void)
{
	IICStart(&temp_pins);
	IICSendByte(&temp_pins,0x70);
	IICWaitAck(&temp_pins);
	IICSendByte(&temp_pins,0xBA);
	IICWaitAck(&temp_pins);
	IICStop(&temp_pins);
}

uint8_t temp_init(void)
{
	TEMP_CLK_ENABLE;
	IICInit(&temp_pins);
	
	delay_ms(40);
	
	if(temp_read_status()&&0x08!=0x08)
	{
		IICStart(&temp_pins);
		IICSendByte(&temp_pins,0x70);
		IICWaitAck(&temp_pins);
		IICSendByte(&temp_pins,0xBE);
		IICWaitAck(&temp_pins);
		IICSendByte(&temp_pins,0x08);
		IICWaitAck(&temp_pins);
		IICSendByte(&temp_pins,0x00);
		IICWaitAck(&temp_pins);
		IICStop(&temp_pins);
		delay_ms(10);
	}
		
	return 0;
}	
 
uint8_t temp_read(float *humi, float *temp)
{
	uint8_t cnt=5;
	uint8_t  Byte_1th=0;
	uint8_t  Byte_2th=0;
	uint8_t  Byte_3th=0;
	uint8_t  Byte_4th=0;
	uint8_t  Byte_5th=0;
	uint8_t  Byte_6th=0;
	uint32_t RetuData = 0;
	
	IICStart(&temp_pins);
	IICSendByte(&temp_pins,0x70);
	IICWaitAck(&temp_pins);
	IICSendByte(&temp_pins,0xAC);
	IICWaitAck(&temp_pins);
	IICSendByte(&temp_pins,0x33);
	IICWaitAck(&temp_pins);
	IICSendByte(&temp_pins,0x00);
	IICWaitAck(&temp_pins);
	IICStop(&temp_pins);	
	
	delay_ms(80);
	while(temp_read_status()&0x80==0x80 && cnt)
	{
		delay_ms(5);
		cnt--;
		temp_read_status();
	}
	if(!cnt)
	{return 1;}
	
	IICStart(&temp_pins);
	IICSendByte(&temp_pins,0x71);
	IICWaitAck(&temp_pins);
	Byte_1th = IICReceiveByte(&temp_pins);
	IICSendAck(&temp_pins);
	Byte_2th = IICReceiveByte(&temp_pins);
	IICSendAck(&temp_pins);
	Byte_3th = IICReceiveByte(&temp_pins);
	IICSendAck(&temp_pins);
	Byte_4th = IICReceiveByte(&temp_pins);
	IICSendAck(&temp_pins);
	Byte_5th = IICReceiveByte(&temp_pins);
	IICSendAck(&temp_pins);
	Byte_6th = IICReceiveByte(&temp_pins);
	IICSendNotAck(&temp_pins);
	IICStop(&temp_pins);

	RetuData = (RetuData|Byte_2th)<<8;
	RetuData = (RetuData|Byte_3th)<<8;
	RetuData = (RetuData|Byte_4th);
	RetuData =RetuData >>4;
	*humi = (RetuData * 1000 >> 20);
	*humi /= 10;
	
	RetuData = 0;
	RetuData = (RetuData|(Byte_4th&0x0f))<<8;
	RetuData = (RetuData|Byte_5th)<<8;
	RetuData = (RetuData|Byte_6th);
	RetuData = RetuData&0xfffff;
	*temp = ((RetuData * 2000 >> 20)- 500);
	*temp /= 10;
	
	return 0;
}
