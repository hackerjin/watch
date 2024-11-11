
void HardwareInitTask(void *argument)
{
    
    delay_init();
    LCD_Init();
    LCD_Set_Light(50);
    LCD_Open_Light();

    LCD_Fill(0,0, LCD_W, LCD_H, BROWN);
    delay_ms(10);
    
    
}
