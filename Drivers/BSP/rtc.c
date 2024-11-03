#include "stm32f4xx_hal.h"

RTC_HandleTypeDef hrtc;

void MX_RTC_Init(void)
{

  /* USER CODE BEGIN RTC_Init 0 */

  /* USER CODE END RTC_Init 0 */

  RTC_TimeTypeDef sTime = {0};
  RTC_DateTypeDef sDate = {0};


  hrtc.Instance = RTC;
  hrtc.Init.HourFormat = RTC_HOURFORMAT_24;
  hrtc.Init.AsynchPrediv = 127;
  hrtc.Init.SynchPrediv = 255;
  hrtc.Init.OutPut = RTC_OUTPUT_DISABLE;
  hrtc.Init.OutPutPolarity = RTC_OUTPUT_POLARITY_HIGH;
  hrtc.Init.OutPutType = RTC_OUTPUT_TYPE_OPENDRAIN;
  HAL_RTC_Init(&hrtc);
 

  if(HAL_RTCEx_BKUPRead(&hrtc,RTC_BKP_DR0)!=0X5050)//是否第一次配置
  {
      sTime.Hours = 0x19;
      sTime.Minutes = 0x14;
      sTime.Seconds = 0x0;
      sTime.DayLightSaving = RTC_DAYLIGHTSAVING_NONE;
      sTime.StoreOperation = RTC_STOREOPERATION_RESET;
      HAL_RTC_SetTime(&hrtc, &sTime, RTC_FORMAT_BCD); 
     
      sDate.WeekDay = RTC_WEEKDAY_SUNDAY;
      sDate.Month = RTC_MONTH_NOVEMBER;
      sDate.Date = 0x3;
      sDate.Year = 0x24;

      HAL_RTC_SetDate(&hrtc, &sDate, RTC_FORMAT_BCD);
     
      HAL_RTCEx_SetWakeUpTimer_IT(&hrtc, 2000, RTC_WAKEUPCLOCK_RTCCLK_DIV16) ;
      
      HAL_RTCEx_BKUPWrite(&hrtc,RTC_BKP_DR0,0X5050);
  }

}


void HAL_RTC_MspInit(RTC_HandleTypeDef* hrtc)
{
  RCC_PeriphCLKInitTypeDef PeriphClkInitStruct = {0};
  if(hrtc->Instance==RTC)
  {
    __HAL_RCC_RTC_ENABLE();
      
    PeriphClkInitStruct.PeriphClockSelection = RCC_PERIPHCLK_RTC;
    PeriphClkInitStruct.RTCClockSelection = RCC_RTCCLKSOURCE_LSE;
    HAL_RCCEx_PeriphCLKConfig(&PeriphClkInitStruct);
  
    HAL_NVIC_SetPriority(RTC_WKUP_IRQn, 5, 0);
    HAL_NVIC_EnableIRQ(RTC_WKUP_IRQn);
  }

}


void HAL_RTC_MspDeInit(RTC_HandleTypeDef* hrtc)
{
  if(hrtc->Instance==RTC)
  {
 
    __HAL_RCC_RTC_DISABLE();
    HAL_NVIC_DisableIRQ(RTC_WKUP_IRQn);
  }

}


uint8_t weekday_calculate(int y,int m,int d,int c)
{
	int w;
	if (m == 1 || m == 2)
	{y--, m += 12;}
	w = y + y / 4 + c / 4  + 26*(m + 1)/10 + d - 1 - 2 * c;
	while(w<0)
		w+=7;
	w%=7;
	w=(w==0)?7:w;
	return w;
}

void RTC_SetTime(uint8_t hours, uint8_t minutes, uint8_t seconds)
{
	RTC_TimeTypeDef time={0};//不设置为{0}时间就会离谱
	time.Hours=hours;
	time.Minutes=minutes;
	time.Seconds=seconds;
	HAL_RTC_SetTime(&hrtc, &time, RTC_FORMAT_BIN);

}

void RTC_SetDate(uint8_t year, uint8_t month, uint8_t date)
{
	RTC_DateTypeDef setdate={0};//不设置为{0}时间就会离谱
	setdate.Date=date;
	setdate.Month=month;
	setdate.Year=year;

	setdate.WeekDay = weekday_calculate(year,month,date,20);

	HAL_RTC_SetDate(&hrtc, &setdate, RTC_FORMAT_BIN) ;
 
}


