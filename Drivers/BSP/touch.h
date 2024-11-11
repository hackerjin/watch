#ifndef __touch_H
#define __touch_H

#include <stdint.h>
#include "iic.h"
#include "delay.h"

//��λ�˿�
#define TOUCH_RST_PORT GPIOA
#define TOUCH_RST_PIN GPIO_PIN_15


//�ж϶˿�
#define TOUCH_INT_PORT GPIOB
#define TOUCH_INT_PIN GPIO_PIN_1


//��λ0
#define TOUCH_RST_0  HAL_GPIO_WritePin(TOUCH_RST_PORT, TOUCH_RST_PIN, GPIO_PIN_RESET)
//��λ1
#define TOUCH_RST_1  HAL_GPIO_WritePin(TOUCH_RST_PORT, TOUCH_RST_PIN, GPIO_PIN_SET)

/* �豸��ַ */
#define Device_Addr 	0x15

/* �������Ĵ��� */
#define GestureID 			0x01
#define FingerNum 			0x02
#define XposH 					0x03
#define XposL 					0x04
#define YposH 					0x05
#define YposL 					0x06
#define ChipID 					0xA7
#define SleepMode				0xE5
#define MotionMask 			0xEC
#define IrqPluseWidth 	0xED
#define NorScanPer 			0xEE
#define MotionSlAngle 	0xEF
#define LpAutoWakeTime 	0xF4
#define LpScanTH 				0xF5
#define LpScanWin 			0xF6
#define LpScanFreq 			0xF7
#define LpScanIdac 			0xF8
#define AutoSleepTime 	0xF9
#define IrqCtl 					0xFA
#define AutoReset 			0xFB
#define LongPressTime 	0xFC
#define IOCtl 					0xFD
#define DisAutoSleep 		0xFE

//����������
typedef struct
{
	unsigned int X_Pos;
	unsigned int Y_Pos;
} touchPos;

//����ʶ��ID
typedef enum
{
	NOGESTURE = 	0x00,
	DOWNGLIDE = 	0x01,
	UPGLIDE = 		0x02,
	LEFTGLIDE = 	0x03,
	RIGHTGLIDE = 	0x04,
	CLICK = 			0x05,
	DOUBLECLICK = 0x0B,
	LONGPRESS = 	0x0C,
} gestureID;

/* ������������ѡ�� */
typedef enum
{
	M_DISABLE = 	0x00,
	EnConLR = 		0x01,
	EnConUD = 		0x02,
	EnDClick = 		0x03,
	M_ALLENABLE = 0x07,
} motionMask;

/* �жϵ����巢�䷽ʽѡ�� */
typedef enum
{
	OnceWLP = 		0x00,
	EnMotion = 		0x10,
	EnChange = 		0x20,
	EnTouch = 		0x40,
	EnTest = 			0x80,
} irqCtl;


/* ��������ʼ����غ��� */
void touch_gpio_init(void);
void touch_reset(void);
void touch_init(void);

/* �������������� */
void touch_get_xy_axis(void);
uint8_t touch_get_chipid(void);
uint8_t touch_get_fingernum(void);

/* �������йز������ú��� */
void touch_config_autosleeptime(uint8_t time);
void touch_wakeup(void);
void touch_sleep(void);

/* ��������д���� */
void touch_iic_writereg(uint8_t addr, uint8_t dat);
uint8_t touch_iic_readreg(unsigned char addr);

#endif
