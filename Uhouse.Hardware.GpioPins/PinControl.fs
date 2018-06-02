namespace Uhouse.Hardware.PinControl
// GPIO - General purpose input/output

type PinId = int

type IPinControl =
    abstract IsEnabled : PinId -> bool
    abstract TurnOn: PinId -> unit
    abstract TurnOff: PinId -> unit

module PinControlFactory = 
    open Unosquare.RaspberryIO
    open Unosquare.RaspberryIO.Gpio

    let getDummyPinControl() = 
        { new IPinControl with
              member __.IsEnabled _: bool = true
              member __.TurnOff _ : unit = ()
              member __.TurnOn _: unit = ()
        }

    let getPinControl() : IPinControl =
        let run f pinId = 
            let pin = Pi.Gpio.Pins.Item pinId
            pin.PinMode <- GpioPinDriveMode.Output 
            f pin

        { new IPinControl with
            member __.IsEnabled pinId = run (fun pin -> pin.Read()) pinId

            member __.TurnOn pinId: unit = run (fun pin -> pin.Write GpioPinValue.High) pinId            

            member __.TurnOff pinId : unit = run (fun pin -> pin.Write GpioPinValue.Low) pinId
        }