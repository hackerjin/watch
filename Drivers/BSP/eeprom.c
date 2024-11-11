#include "eeprom.h"
#include "stm32f4xx_hal.h"
#include "iic.h"



#define EEPROM_CLK_ENABLE __HAL_RCC_GPIOA_CLK_ENABLE()

iic_bus_t eeprom_pins = 
{
	.IIC_SDA_PORT = GPIOA,
	.IIC_SCL_PORT = GPIOA,
	.IIC_SDA_PIN  = GPIO_PIN_11,
	.IIC_SCL_PIN  = GPIO_PIN_12,
};


void eeprom_write(uint8_t addr,uint8_t length,uint8_t buff[])
{
	IIC_Write_Multi_Byte(&eeprom_pins, EEPROM_ADDRESS, addr, length, buff);
}


void eeprom_read(uint8_t addr, uint8_t length, uint8_t buff[])
{
	IIC_Read_Multi_Byte(&eeprom_pins, EEPROM_ADDRESS, addr, length, buff);
}


void eeprom_init(void)
{
	EEPROM_CLK_ENABLE;
	IICInit(&eeprom_pins);
}
