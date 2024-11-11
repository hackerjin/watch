#ifndef __temp21_H
#define __temp21_H

#include "stm32f4xx_hal.h"
#include <stdint.h>

uint8_t temp_read_status(void);
void temp_reset(void);
uint8_t temp_init(void);
uint8_t temp_read(float *humi, float *temp);

#endif
