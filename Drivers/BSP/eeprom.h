#ifndef __BL24C02_H
#define __BL24C02_H

#include <stdint.h>

#define EEPROM_ADDRESS	0x50

void eeprom_write(uint8_t addr,uint8_t length,uint8_t buff[]);
void eeprom_read(uint8_t addr, uint8_t length, uint8_t buff[]);
void eeprom_init(void);

#endif
