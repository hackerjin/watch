#include "compass.h"
#include "iic.h"
#include <math.h>

#define PI 3.1415926
#define Delayms(X) HAL_Delay(X)

#define LSM303_CLK_ENABLE __HAL_RCC_GPIOB_CLK_ENABLE()

iic_bus_t LSM303_bus = 
{
	.IIC_SDA_PORT = GPIOB,
	.IIC_SCL_PORT = GPIOB,
	.IIC_SDA_PIN  = GPIO_PIN_13,
	.IIC_SCL_PIN  = GPIO_PIN_14,
};


//加速度和磁力计的寄存器地址统一编址，所以需要根据大小判断访问的是哪一个部件
unsigned char LSM303_ReadOneReg(unsigned char RegAddr)
{
	unsigned char dat;
	unsigned char SlaveAddr = (RegAddr>0x19)?LSM303_SlaveAddr_A:LSM303_SlaveAddr_M;
	dat = IIC_Read_One_Byte(&LSM303_bus, SlaveAddr,RegAddr);
	return dat;
}


/*************************************************************************************************************************
*function  	:	LSM303_WriteOneReg()
*paraments	: RegAddr, data
*return     : None
*detail     :	
*************************************************************************************************************************/
void LSM303_WriteOneReg(unsigned char RegAddr, unsigned char dat)
{
	unsigned char SlaveAddr = (RegAddr>0x19)?LSM303_SlaveAddr_A:LSM303_SlaveAddr_M;
	IIC_Write_One_Byte(&LSM303_bus, SlaveAddr,RegAddr,dat);
}


//连续读取多个寄存器的值
void LSM303_ReadMultiReg(unsigned char RegAddr, unsigned char RegNum, unsigned char DataBuff[])
{
	unsigned char i;
	unsigned char SlaveAddr = (RegAddr>0x19)?LSM303_SlaveAddr_A:LSM303_SlaveAddr_M;
	for(i=0;i<RegNum;i++)
	{
		DataBuff[i] = IIC_Read_One_Byte(&LSM303_bus, SlaveAddr,RegAddr+i);
	}
}


/*************************************************************************************************************************
*function  	:	LSM303DLH_Init()
*paraments	: None
*return     : succeed or not
*detail     :	the defalt CTRL_REG4 value is 0x00. FS[5:4], 00: ±2 g, 01: ±4 g, 10: ±8 g, 11: ±16 g
*************************************************************************************************************************/
unsigned char compass_init()
{
	unsigned char temp;
	unsigned char retry = 0;
	
	LSM303_CLK_ENABLE;
	IICInit(&LSM303_bus);
	//加速度
	for(retry = 0;retry < 3;retry ++)
	{
		LSM303_WriteOneReg(LSM303_CTRL_REG4_A, 0x10);		//full scale: +-4g
		Delayms(1);
		LSM303_WriteOneReg(LSM303_CTRL_REG1_A, 0x2F);		//low power mode, 10Hz速度
		Delayms(1);															
		temp = LSM303_ReadOneReg(LSM303_CTRL_REG1_A);		//读取0x20寄存器，默认值为0x07
		if(temp != 0x2F)	
		{
			Delayms(10);	
		}
		else break;
	}
	if(temp != 0x2F)
	{
		return 1;
	}
	Delayms(1);	
        
    
	for(retry = 0;retry < 3;retry ++)
	{
		LSM303_WriteOneReg(LSM303_CRA_REG_M, 0x10);		//磁场传感器15Hz,正常测量;关闭温度计
		Delayms(1);		
		LSM303_WriteOneReg(LSM303_CRB_REG_M, 0x80);		//磁场传感器gain默认为0x80: +-4.0Gauss, 450LBS/Gauss for XY, 400LBS/Gauss for Z
		Delayms(1);	
		LSM303_WriteOneReg(LSM303_MR_REG_M, 0x00);		//磁场传感器连续转换模式
		Delayms(1);										
		temp = LSM303_ReadOneReg(LSM303_MR_REG_M);		
		if(temp != 0)	
		{
			Delayms(10);	
		}
		else break;
	}
	if(temp != 0)	
	{
		return 1;	
	}
	return 0;
}

/*************************************************************************************************************************
*function  	:	LSM303DLH_Sleep()
*paraments	: None
*return     : None
*detail     :	
*************************************************************************************************************************/
void compass_sleep()
{
	LSM303_WriteOneReg(LSM303_MR_REG_M,0x03);
	LSM303_WriteOneReg(LSM303_CTRL_REG1_A,0x0f);		
}

/*************************************************************************************************************************
*function  	:	LSM303DLH_Wakeup()
*paraments	: None
*return     : None
*detail     :	
*************************************************************************************************************************/
void compass_wakeup()
{
	LSM303_WriteOneReg(LSM303_MR_REG_M,0x00);//磁场传感器连续转换模式
	LSM303_WriteOneReg(LSM303_CTRL_REG1_A,0x2f);//low power mode, 10Hz速度	
}

/*************************************************************************************************************************
*function  	:	LSM303_ReadAcceleration()
*paraments	: int16_t *Xa, int16_t *Ya, int16_t *Z
*return     : None
*detail     :	to read the Acceleration data, but the real Acceleration should be divided by the unit(n LSB/A) 
*************************************************************************************************************************/
void compass_readacceleration(int16_t *Xa, int16_t *Ya, int16_t *Za)
{
	uint8_t buff[6];
	int16_t temp;
	LSM303_ReadMultiReg(LSM303_OUT_X_L_A,6,buff);
	
	temp = buff[1];
	temp <<= 8;
	temp |= buff[0];
	*Xa = temp;
	
	temp = buff[3];
	temp <<= 8;
	temp |= buff[2];
	*Ya = temp;
	
	temp = buff[5];
	temp <<= 8;
	temp |= buff[4];
	*Za = temp;
}

/*************************************************************************************************************************
*function  	:	LSM303_ReadMagnetic()
*paraments	: int16_t *Xm, int16_t *Ym, int16_t *Zm
*return     : None
*detail     :	to read the Magnetic data, but the real Magnetic should be divided by the unit(n LSB/M) 
*************************************************************************************************************************/
void compass_readmagnetic(int16_t *Xm, int16_t *Ym, int16_t *Zm)
{
	uint8_t buff[6];
	int16_t temp;
	LSM303_ReadMultiReg(LSM303_OUT_X_H_M,6, buff);
	
	temp = buff[0];
	temp <<= 8;
	temp |= buff[1];
	*Xm = temp;
	
	temp = buff[2];
	temp <<= 8;
	temp |= buff[3];
	*Zm = temp;
	
	temp = buff[4];
	temp <<= 8;
	temp |= buff[5];
	*Ym = temp;
}



/*************************************************************************************************************************
*function  	:	Azimuth_Calculate()
*paraments	: int16_t Xa, int16_t Ya, int16_t Za, int16_t Xm, int16_t Ym, int16_t Zm
*return     : Azimuth
*detail     :	to Calculate the Azimuth to find the direction
*************************************************************************************************************************/
float azimuth_calculate(int16_t Xa, int16_t Ya, int16_t Za, int16_t Xm, int16_t Ym, int16_t Zm)
{
	float pitch, roll, Hy, Hx, Azimuth; 
	pitch   = atan2f(Xa, sqrtf(Ya * Ya + Za * Za));
	roll    = atan2f(Ya, sqrtf(Xa * Xa + Za * Za));
	Hy      = Ym * cosf(roll) + Xm * sinf(roll) * sinf(pitch) - Zm * cosf(pitch) * sinf(roll);
	Hx      = Xm * cosf(pitch) + Zm * sinf(pitch);
	Azimuth = atan2f(Hy,Hx)*180.0/PI;
	return Azimuth;
}
