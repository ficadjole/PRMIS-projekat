# Formula 1 Telemetry Simulation  
A simulation system for modeling telemetry communication between Formula 1 cars, garages, and race control.

## Project Overview  
This project simulates a Formula 1 telemetry environment where a car sends status information to its garage, receives driving instructions, and forwards lap times to race control.  
The system showcases TCP/UDP communication, socket multiplexing, lap time calculation logic, basic data encryption, and real-time simulation of car behavior.

---

# Authors  

- [Filip Velemir](https://github.com/ficadjole)
- [Maja Bogičević](https://github.com/MajaBogicevic)
  
---

## System Features

### Car (Client)
Each car behaves as an independent client with the following capabilities:  
- Select manufacturer: Mercedes, Ferrari, Renault, Honda  
- Manufacturer defines base fuel and tire consumption  
- Stores tire compounds:
  - Soft (M) – 80 km  
  - Medium (S) – 100 km  
  - Hard (T) – 120 km  
- Opens a TCP connection to the garage  
- Opens a UDP socket for driving instructions  
- Sends telemetry updates to garage and lap times to race control  

### Garage (Client + Server)
The garage acts as both a TCP server and a UDP client. It:  
- Opens a TCP socket and accepts up to two cars (socket multiplexing)  
- Collects track length and base lap time from the user  
- Sends tire and fuel configuration to the car before it enters the track  
- Sends driving mode instructions (fast, medium, slow) via UDP  
- Can call the car to enter or return from the track  

### Race Control (Server)
Race control receives data from all active cars:  
- Receives lap times via TCP  
- Stores lap times in a dictionary keyed by race number and manufacturer  
- Displays and updates the list of fastest laps  
- Tracks whether each car is on track or in the garage  

---

## Communication Model

### TCP (Reliable)
Used for:  
- Car → Garage (fuel, tire wear, engine usage)  
- Car → Race Control (lap times, active status)  
- Garage → Car (initial data exchange)

### UDP (Low Latency)
Used for:  
- Garage → Car driving instructions  
- Tire/fuel configuration messages  
- Track entry/exit commands  

---

## Data Processing & Logic

### Lap Time Calculation  
Lap time depends on:  
- Base lap time  
- Remaining fuel  
- Tire wear  
- Driving mode

Formula: LapTime = baseTime - fuelTempo - tireTempo

Driving mode effect:  
- Fast: increases fuel & tire wear  
- Medium: normal values  
- Slow: increases lap time slightly each lap  

### Tire Tempo Rules  
- Soft: 1.2 × lap number  
- Medium: 1 × lap number  
- Hard: 0.8 × lap number  
- Below 40% tire durability → performance penalty  

### Fuel Tempo
Formula: fuelTempo = 1 / currentFuel


---

## Used Technologies
- C# 
- TCP and UDP socket programming  
- Multithreading 
- Socket multiplexing (select/poll)  
- Console-based interface  
