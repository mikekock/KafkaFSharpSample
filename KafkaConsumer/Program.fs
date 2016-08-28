module MainKafkaConsumer
open System
open System.Text
open System.Collections.Generic
open KafkaConsumerUtil
open RdKafka
open EventTypeJSON
open ItemEvents

type Input = 
    | Created of ItemCreated
    | DescChanged of ItemDescriptionChanged
    | Deleted of ItemDeleted

type Output = 
    | StatusMessage of string

let handle input =
    match input with
        | Created c ->  Some(Output.StatusMessage (sprintf "Created %s %s %s %s" c.Id c.Description (c.Audit.TimestampUTC.ToLongDateString()) (c.Audit.TimestampUTC.ToLongTimeString())))
        | DescChanged c ->  Some(Output.StatusMessage (sprintf "Changed %s %s %s %s" c.Id c.Description (c.Audit.TimestampUTC.ToLongDateString()) (c.Audit.TimestampUTC.ToLongTimeString())))
        | Deleted c ->  Some(Output.StatusMessage (sprintf "Deleted %s %s %s" c.Id (c.Audit.TimestampUTC.ToLongDateString()) (c.Audit.TimestampUTC.ToLongTimeString())))
                        
let interpret output =
    match output with
    | StatusMessage s -> 
        printfn "%s" s
        Some(true)
                        
let decode msg =
    let eventType = parseEventTypePayload msg
    match eventType.EventType with
    | "ItemCreated" ->
        Some(Input.Created(ItemCreated.fromJSON(msg)))
    | "ItemDescriptionChanged" ->
        Some(Input.DescChanged(ItemDescriptionChanged.fromJSON(msg)))
    | "ItemDeleted" ->
        Some(Input.Deleted(ItemDeleted.fromJSON(msg)))
    | _ ->
        None

[<EntryPoint>]
let main argv = 
    let consumerGroup = argv.[0]

    let pipeline message = standardSomeOrNonePipeline decode handle interpret message

    use consumer = kafkaConsumer consumerGroup "www.smokinserver.com:9092" "test" pipeline

    printfn "Started consumer, press enter to stop consuming" 
    
    let unused = Console.ReadLine()
    consumer.Stop() |> Async.AwaitTask |> ignore

    printfn "%s" consumer.Name
    0 // return an integer exit code

