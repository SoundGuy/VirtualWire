// receiver.pde
//
// Simple example of how to use VirtualWire to receive messages
// Implements a simplex (one-way) receiver with an Rx-B1 module
//
// See VirtualWire.h for detailed API docs
// Author: Mike McCauley (mikem@airspayce.com)
// Copyright (C) 2008 Mike McCauley
// $Id: receiver.pde,v 1.3 2009/03/30 00:07:24 mikem Exp $

#include <VirtualWire.h>
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

#define PIN 0
Adafruit_NeoPixel strip = Adafruit_NeoPixel(10, PIN, NEO_GRB + NEO_KHZ800);


const int led_pin = 3;
const int receive_pin = 4;

void setup()
{
  // This is for Trinket 5V 16MHz, you can remove these three lines if you are not using a Trinket
  #if defined (__AVR_ATtiny85__)
    if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
  #endif
  // End of trinket special code
  //pinMode(4, INPUT); 
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'

  colorWipe(strip.Color(0, 0, 255), 25); // Red 

    delay(200);

pinMode(led_pin,OUTPUT); 
digitalWrite(led_pin, HIGH); // Flash a light to show received good message
        
        delay(250);
        digitalWrite(led_pin, LOW);
    
colorWipe(strip.Color(0, 0, 0), 0); // blank
    // Initialise the IO and ISR
    vw_set_rx_pin(receive_pin);
    vw_setup(2000);	 // Bits per sec

    vw_rx_start();       // Start the receiver PLL running

    //pinMode(led_pin, OUTPUT);
}


// Fill the dots one after the other with a color
void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i=0; i<strip.numPixels(); i++) {
    strip.setPixelColor(i, c);
    strip.show();
    delay(wait);
  }
}


void loop()
{
    uint8_t buf[VW_MAX_MESSAGE_LEN];
    uint8_t buflen = VW_MAX_MESSAGE_LEN;

    if (vw_get_message(buf, &buflen)) // Non-blocking
    {
	

       // Message with a good checksum received, dump it.
      if (buf[0] == 'r') {
        colorWipe(strip.Color(255, 0, 0), 0); // Red 
      
      }
      
      if (buf[0] == 'g') {
        colorWipe(strip.Color(0, 255, 0), 0); // green
        }

 digitalWrite(led_pin, HIGH); // Flash a light to show received good message
        
        delay(150);
	      digitalWrite(led_pin, LOW);
    }
    
    digitalWrite(led_pin, LOW);
}
