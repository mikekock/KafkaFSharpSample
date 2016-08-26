// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open System
open System.Text
open System.Collections.Generic
open RdKafka

type Input = 
    | Val of string

//type Battle with
//    static member fromJSON json =
//        let jo = JObject.Parse(json)
//        let result:Battle = {
//            Dtstamp = Guid(jo.["Dtstamp"].ToString()); 
//            Id = Guid(jo.["Id"].ToString()); 
//            Attacker = Guid(jo.["Attacker"].ToString()); 
//            Defender = Guid(jo.["Defender"].ToString()); 
//            Winner = Guid(jo.["Winner"].ToString()); 
//            Loser = Guid(jo.["Loser"].ToString());
//            }
//        result

type Output = 
    | Result of string

let handle input =
    match input with
        | Val v ->  Some(Output.Result (sprintf "Output %s" v))
            
let interpret output =
    match output with
    | Result r -> 
        printfn "%s" r
        Some(true)
                        
let decode msg =
    //let eventType = parseEventTypePayload msg
    Some(Input.Val(msg))
    //match eventType.EventType with
    //| "Battle" ->
    //    Some(Input.Bat(Battle.fromJSON(eventType.Payload)))
    //| _ -> 
    //    printfn "Encountered unknown EventType %s" eventType.EventType
    //    None



let kafkaConsumer group server topic pipeline = 
    let config = new Config()
    config.GroupId <- group

    let consumer:EventConsumer = new EventConsumer(config, server)
    let decodeKafkaMessageToString (msg:Message) = 
        Encoding.UTF8.GetString(msg.Payload, 0, msg.Payload.Length)
    let pipe msg = decodeKafkaMessageToString msg |> pipeline |> ignore
    
    consumer.OnMessage.Add(fun msg -> msg |> pipe |> ignore) 
    let topics = new List<string>()
    topics.Add topic
    consumer.Subscribe topics
    consumer.Start()
    consumer

let (>>=) m f = Option.bind f m

let standardSomeOrNonePipeline decodeInput handleInput interpretOutput message = 
    message 
        |> decodeInput
        >>= handleInput
        >>= interpretOutput


[<EntryPoint>]
let main argv = 
    let consumerGroup = argv.[0]

    let pipeline message = standardSomeOrNonePipeline decode handle interpret message

    let consumer = kafkaConsumer consumerGroup "www.smokinserver.com:9092" "test" pipeline

    printfn "Started consumer, press enter to stop consuming" 
    
    let unused = Console.ReadLine()

    printfn "%s" consumer.Name
    0 // return an integer exit code

