let BaseUri = "http://192.168.0.103:8000"
let SnakeDelay = 200

open System.Net
open System

let post uri =        
    let req = WebRequest.CreateHttp(Uri(uri),Method="POST",ContentLength=0L) 
    use r = req.GetResponse()
    ()
   
type PinAction = | On | Off

let getPinActionUri (pinAction: PinAction) (pinId: int) =
    sprintf "%s/api/pin/%d/%A" BaseUri pinId pinAction

let changePinState pinAction pinId = getPinActionUri pinAction pinId |> post

let on = changePinState On
let off = changePinState Off

let PinIds = [ 0 .. 5 ]

let delay t action = 
    action
    System.Threading.Thread.Sleep(t: int)

let rec blinkySnake(): unit =
    List.iter (on  >> delay SnakeDelay) PinIds
    List.iter (off >> delay SnakeDelay) PinIds
    blinkySnake()

blinkySnake()