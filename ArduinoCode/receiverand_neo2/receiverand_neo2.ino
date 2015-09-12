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
Adafruit_NeoPixel strip = Adafruit_NeoPixel(11, PIN, NEO_GRB + NEO_KHZ800);


const int led_pin = 3;
const int receive_pin = 2;
const int analog_pin = 2; //PB4
const int unit_id = 0; // for odd/even reference, etc.

int sensorValue=0;
int bpm = 0;


void setup()
{
  // This is for Trinket 5V 16MHz, you can remove these three lines if you are not using a Trinket
  #if defined (__AVR_ATtiny85__)
    if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
  #endif
  // End of trinket special code
  pinMode(analog_pin , INPUT); 
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'

  colorWipe(strip.Color(0, 128, 128), 25); // Red 

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

// Fill all pixels, fading in and out
void colorFadeInOut(uint8_t r0, uint8_t g0, uint8_t b0, uint8_t wait) {
  int num_steps = 16;
  if (wait < num_steps) wait = num_steps; // make sure the delay is not 0
  uint8_t r=0, g=0, b=0;
  float dr = (float)r0 / num_steps;
  float dg = (float)g0 / num_steps;
  float db = (float)b0 / num_steps;
  for (int step = 0; step < 2*num_steps; step++, r+=dr, g+=dg, b+=db) {
    for(uint16_t i=0; i<strip.numPixels(); i++) {
      strip.setPixelColor(i, strip.Color((uint8_t)r, (uint8_t)g, (uint8_t)b));
    }
    strip.show();
    delay(wait/num_steps);
    if (step == num_steps) {
      dr = -dr;
      dg = -dg;
      db = -db;
    }
  }
}

// Fill all pixels, fading in
void colorFadeIn(uint8_t r0, uint8_t g0, uint8_t b0, uint8_t wait) {
  int num_steps = 16;
  if (wait < num_steps) wait = num_steps; // make sure the delay is not 0
  uint8_t r=0, g=0, b=0;
  float dr = (float)r0 / num_steps;
  float dg = (float)g0 / num_steps;
  float db = (float)b0 / num_steps;
  for (int step = 0; step < num_steps; step++, r+=dr, g+=dg, b+=db) {
    for(uint16_t i=0; i<strip.numPixels(); i++) {
      strip.setPixelColor(i, strip.Color((uint8_t)r, (uint8_t)g, (uint8_t)b));
    }
    strip.show();
    delay(wait/num_steps);
  }
  // make sure it's on
  colorWipe(strip.Color((uint8_t)r0, (uint8_t)g0, (uint8_t)b0), 0);
}
/*
// Fill all pixels, fading in and out
void colorFadeOut(uint8_t r0, uint8_t g0, uint8_t b0, uint8_t wait) {
  int num_steps = 16;
  if (wait < num_steps) wait = num_steps; // make sure the delay is not 0
  uint8_t r=r0, g=g0, b=b0;
  float dr = (float)r0 / num_steps;
  float dg = (float)g0 / num_steps;
  float db = (float)b0 / num_steps;
  for (int step = 0; step < num_steps; step++, r-=dr, g-=dg, b-=db) {
    for(uint16_t i=0; i<strip.numPixels(); i++) {
      strip.setPixelColor(i, strip.Color((uint8_t)r, (uint8_t)g, (uint8_t)b));
    }
    strip.show();
    delay(wait/num_steps);
  }
  // make sure it's off
  colorWipe(strip.Color(0,0,0), 0);
}*/

void loop() {
  sensorValue = analogRead(analog_pin );
  if(sensorValue > 10) {
    digitalWrite(led_pin, HIGH); // Flash a light to show received good message
  } 
  else {
    digitalWrite(led_pin, LOW); // Flash a light to show received good message
  }
  uint8_t buf[VW_MAX_MESSAGE_LEN];
  uint8_t buflen = VW_MAX_MESSAGE_LEN;
  
  if (vw_get_message(buf, &buflen)) { // Non-blocking    
    uint8_t msg_id = buf[0];
    switch (msg_id) {
      case 'C': // 67 // turn color constant 'on'
        colorWipe(strip.Color(buf[1], buf[2], buf[3]), 0);        
        break;
      case 'c': // 99 // fade color in and out
        colorFadeInOut(buf[1], buf[2], buf[3], buf[4]);
        break;
      case 'B': // 66 // set bpm
        bpm = buf[1];
        break;
      case 'R': // 82 // call rainbow
        // rainbow starts and ends in red, so fade in and out to make it smoother
        //colorFadeIn(255,0,0,5);
        rainbow(buf[1]);
        //colorFadeOut(255,0,0,5);        
        break;
    }      
  }
}

void rainbow(uint8_t wait) {
  uint16_t i, j;
  int num_steps = 64; //original is 256, but it takes too long
  int step = 256 / num_steps;

  for(j=0; j<256; j+=step) {
    for(i=0; i<strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel((i+j) & 255));
    }
    strip.show();
    delay(wait);
  }
}

// Slightly different, this makes the rainbow equally distributed throughout
void rainbowCycle(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256*5; j++) { // 5 cycles of all colors on wheel
    for(i=0; i< strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel(((i * 256 / strip.numPixels()) + j) & 255));
    }
    strip.show();
    delay(wait);
  }
}

// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
  WheelPos = 255 - WheelPos;
  if(WheelPos < 85) {
   return strip.Color(255 - WheelPos * 3, 0, WheelPos * 3);
  } else if(WheelPos < 170) {
    WheelPos -= 85;
   return strip.Color(0, WheelPos * 3, 255 - WheelPos * 3);
  } else {
   WheelPos -= 170;
   return strip.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
  }
}
