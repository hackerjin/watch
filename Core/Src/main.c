#include "main.h"
#include "cmsis_os.h"
#include "lcd.h"
#include "lcd_init.h"
#include "sys.h"
#include "delay.h"
#include <stdio.h>
#include <stdbool.h>

void SystemClock_Config(void);
void MX_ADC1_Init(void);
void MX_RTC_Init(void);
void MX_TIM3_Init(void);
bool MX_SPI1_Init(void);
void MX_USART1_UART_Init(void);


osThreadId_t HardwareInitTaskHandle;
const osThreadAttr_t HardwareInitTask_attributes = {
  .name = "HardwareInitTask",
  .stack_size = 128 * 10,
  .priority = (osPriority_t) osPriorityHigh3,
};



void HardwareInitTask(void *argument)
{
    

    
    
}

void tasks_init()
{
    HardwareInitTaskHandle  = osThreadNew(HardwareInitTask, NULL, &HardwareInitTask_attributes);
    
}

int main(void)
{

    SCB->VTOR = 0x00000000U;
    HAL_Init();


    SystemClock_Config();
    
    __enable_irq();
    
    MX_ADC1_Init();
    MX_RTC_Init();
    MX_TIM3_Init();
    
    MX_USART1_UART_Init();
    
    //osKernelInitialize();
    delay_init();
    if(MX_SPI1_Init() == HAL_OK)
    {
        while(1)
        {
        LCD_Open_Light();
       
       
        LCD_Init();
        LCD_Fill(0,0, LCD_W, LCD_H, BLACK);
        delay_ms(10);
        LCD_Set_Light(50);
        LCD_ShowString(72,LCD_H/2,(uint8_t*)"Welcome!", WHITE, BLACK, 24, 0);//12*6,16*8,24*12,32*16
        uint8_t lcd_buf_str[17];
        sprintf(lcd_buf_str, "OV-Watch V%d.%d.%d", 1, 1, 1);
        LCD_ShowString(34, LCD_H/2+48, (uint8_t*)lcd_buf_str, WHITE, BLACK, 24, 0);
        delay_ms(1000);
        LCD_Fill(0, LCD_H/2-24, LCD_W, LCD_H/2+49, BLACK);
            
        }
    }
    
    
    //osKernelStart();
    
    while (1)
    {

    }

}


void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

  /** Configure the main internal regulator output voltage
  */
  __HAL_RCC_PWR_CLK_ENABLE();
  __HAL_PWR_VOLTAGESCALING_CONFIG(PWR_REGULATOR_VOLTAGE_SCALE1);

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSI|RCC_OSCILLATORTYPE_LSE;
  RCC_OscInitStruct.LSEState = RCC_LSE_ON;
  RCC_OscInitStruct.HSIState = RCC_HSI_ON;
  RCC_OscInitStruct.HSICalibrationValue = RCC_HSICALIBRATION_DEFAULT;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSI;
  RCC_OscInitStruct.PLL.PLLM = 8;
  RCC_OscInitStruct.PLL.PLLN = 100;
  RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV2;
  RCC_OscInitStruct.PLL.PLLQ = 4;
  HAL_RCC_OscConfig(&RCC_OscInitStruct);
  
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV2;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

  HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_3);
}





void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef *htim)
{
  /* USER CODE BEGIN Callback 0 */

  /* USER CODE END Callback 0 */
  if (htim->Instance == TIM1) {
    HAL_IncTick();
  }
}


