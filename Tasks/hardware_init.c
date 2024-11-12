#include "stm32f4xx_hal.h"
#include "lcd_init.h"
#include "lcd.h"
#include "delay.h"

void hardware_init_task(void *argument)
{
   
    LCD_Init();
    LCD_Set_Light(50);
    LCD_Open_Light();
    LCD_Fill(0,0, LCD_W, LCD_H, YELLOW);
    
    
}
