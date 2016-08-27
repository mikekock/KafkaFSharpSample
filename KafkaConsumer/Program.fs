module MainKafkaConsumer
open System
open System.Text
open System.Collections.Generic
open KafkaConsumerUtil
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





[<EntryPoint>]
let main argv = 
    let consumerGroup = argv.[0]

    let pipeline message = standardSomeOrNonePipeline decode handle interpret message

    use consumer = kafkaConsumer consumerGroup "www.smokinserver.com:9092" "test" pipeline

    printfn "Started consumer, press enter to stop consuming" 
    
    let unused = Console.ReadLine()

    printfn "%s" consumer.Name
    0 // return an integer exit code

