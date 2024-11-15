#ifndef _LSM303_H_
#define _LSM303_H_

#include <stdint.h>
 
//罗盘
#define	LSM303_CRA_REG_M					0x00	//RW
#define	LSM303_CRB_REG_M					0x01	//RW
#define	LSM303_MR_REG_M						0x02	//RW
#define	LSM303_OUT_X_H_M					0x03	//R
#define	LSM303_OUT_X_L_M					0x04	//R
#define	LSM303_OUT_Z_H_M					0x05	//R
#define	LSM303_OUT_Z_L_M					0x06	//R
#define	LSM303_OUT_Y_H_M					0x07	//R
#define	LSM303_OUT_Y_L_M					0x08	//R
#define	LSM303_SR_REG_M						0x09	//R
#define	LSM303_IRA_REG_M					0x0A	//R
#define	LSM303_IRB_REG_M					0x0B	//R
#define	LSM303_IRC_REG_M					0x0C	//R
	
//加速度
#define	LSM303_CTRL_REG1_A					0x20	//RW
#define	LSM303_CTRL_REG2_A					0x21	//RW
#define	LSM303_CTRL_REG3_A					0x22	//RW
#define	LSM303_CTRL_REG4_A					0x23	//RW
#define	LSM303_CTRL_REG5_A					0x24	//RW
#define	LSM303_HP_FILTER_RESET_A		0x25	//R
#define	LSM303_REFERENCE_A					0x26	//RW
#define	LSM303_STATUS_REG_A					0x27	//R
#define	LSM303_OUT_X_L_A						0x28	//R
#define	LSM303_OUT_X_H_A						0x29	//R
#define	LSM303_OUT_Y_L_A						0x2A	//R
#define	LSM303_OUT_Y_H_A						0x2B	//R
#define	LSM303_OUT_Z_L_A						0x2C	//R
#define	LSM303_OUT_Z_H_A						0x2D	//R
#define	LSM303_INT1_CFG_A						0x30	//RW
#define	LSM303_INT1_SOURCE_A				0x31	//R
#define	LSM303_INT1_THS_A						0x32	//RW
#define	LSM303_INT1_DURATION_A			0x33	//RW
#define	LSM303_INT2_CFG_A						0x34	//RW
#define	LSM303_INT2_SOURCE_A				0x35	//R
#define	LSM303_INT2_THS_A						0x36	//RW
#define	LSM303_INT2_DURATION_A			0x37	//RW

//温度
#define TEMP_OUT_H_M								0x31	//R
#define TEMP_OUT_L_M								0x32	//R

#define LSM303_SlaveAddr_A					0x19	//加速度地址
#define LSM303_SlaveAddr_M					0x1E	//磁力计和温度的地址			


unsigned char compass_init(void);

void compass_sleep(void);
void compass_wakeup(void);

void compass_readacceleration(int16_t *Xa, int16_t *Ya, int16_t *Za);
void compass_readmagnetic(int16_t *Xm, int16_t *Ym, int16_t *Zm);


float azimuth_calculate(int16_t Xa, int16_t Ya, int16_t Za, int16_t Xm, int16_t Ym, int16_t Zm);

#endif
