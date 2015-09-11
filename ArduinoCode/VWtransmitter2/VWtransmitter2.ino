// transmitter.pde
//
// Simple example of how to use VirtualWire to transmit messages
// Implements a simplex (one-way) transmitter with an TX-C1 module
//
// See VirtualWire.h for detailed API docs
// Author: Mike McCauley (mikem@airspayce.com)
// Copyright (C) 2008 Mike McCauley
// $Id: transmitter.pde,v 1.3 2009/03/30 00:07:24 mikem Exp $

#include <VirtualWire.h>

const int led_pin = 13;
const int transmit_pin = 3;
//const int receive_pin = 2;
const int transmit_en_pin = 3;

int incomingByte = 0;   // for incoming serial data
String incoming_string; // for incoming serial data

void setup()
{
    // Initialise the IO and ISR
    vw_set_tx_pin(transmit_pin);
   // vw_set_rx_pin(receive_pin);
    vw_set_ptt_pin(transmit_en_pin);
    vw_set_ptt_inverted(true); // Required for DR3100
    vw_setup(2000);       // Bits per sec
    pinMode(led_pin, OUTPUT);
    Serial.begin(9600); // for reading cmd from pc
}

byte count = 1;

void loop()
{  
  int msg_len = Serial.available();
  if (msg_len > 0) {
    digitalWrite(led_pin, HIGH); // Flash a light to show transmitting
    //Serial.print(msg_len);
    unsigned char msg[msg_len]; // = {67,0,0,255};
    Serial.readBytes(msg, msg_len);
    vw_send((uint8_t *)msg, msg_len);
    /*
    int r, g, b;
    char msg_id = Serial.read();
    switch (msg_id) {
      case 'C':
        r = Serial.read();
        g = Serial.read();
        b = Serial.read();        
        
        vw_send((uint8_t *)msg, 3);
        break;
    } 
    */   
    vw_wait_tx(); // Wait until the whole message is gone
    
    digitalWrite(led_pin, LOW);
    //delay(500);
  }
  /*
  else {
    char msg[2] = {'g',' '};
  
    digitalWrite(led_pin, HIGH); // Flash a light to show transmitting
    vw_send((uint8_t *)msg, 2);
    vw_wait_tx(); // Wait until the whole message is gone
    digitalWrite(led_pin, LOW);
    delay(500);
  
    char msg2[2] = {'r',' '};
  
    digitalWrite(led_pin, HIGH); // Flash a light to show transmitting
    vw_send((uint8_t *)msg2, 2);
    vw_wait_tx(); // Wait until the whole message is gone
    digitalWrite(led_pin, LOW);
    delay(500);
  }
  */
}
