﻿// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
open System
open System.Text
open RdKafka

let stringToUTF8Bytes (s:string) =
    let data = Encoding.UTF8.GetBytes(s)
    data

let sendToTopic (topic:Topic) (message:byte[]) =
    message |> topic.Produce

let sendToTopicWithKey (topic:Topic) (message:byte[], key:byte[]) =
     topic.Produce (message, key)

let createKafkaTopic serverName bufferingMS topicName : Topic = 
    let c = new Config()
    c.["queue.buffering.max.ms"] <- bufferingMS;
    let producer = new Producer(serverName)
    let topic = producer.Topic(topicName)
    topic

let kafkaTopic topicName : Topic = 
    createKafkaTopic "www.smokinserver.com:9092" "10" topicName

[<EntryPoint>]
let main argv = 
    let iterations = System.Int32.Parse argv.[0]
    use topic = kafkaTopic("test")
    let sendToTestTopic = sendToTopic topic
    let rnd = System.Random()

    for i in 1 .. iterations do

        let randomNumber = rnd.Next 9
        let s = sprintf "Hello #%d" i
        let msg = stringToUTF8Bytes s
        let key = stringToUTF8Bytes s
 
        //msg |> topic.Produce |> ignore
        sendToTestTopic msg |> ignore
        printfn "%d = %d" i randomNumber

    
    //let c = new Config()
    //                                            c.["queue.buffering.max.ms"] <- "1";
    //                                            let tc = new TopicConfig()
    //                                            let producer = new Producer("www.smokinserver.com:9092")
    //                                            let topic = producer.Topic("battlers")
    //                                            
    //                                            (*let f = JsonConvert.SerializeObject({EventType = "XYZ"; Payload = battle})
    //                                            Encoding.UTF8.GetBytes(f)
    //                                            |> topic.Produce |> ignore*)
    //
    //                                            let envelope:EventEnvelope<Battle> = {EventType = "Battle"; Payload = battle}
    //                                            let e = JsonConvert.SerializeObject(envelope)
    //                                            printfn "Sending to Kafka -> %s" e
    //                                            Encoding.UTF8.GetBytes(e)
    //                                            |> topic.Produce |> ignore
    let unused = Console.ReadLine()
    printfn "%s" topic.Name
    0 // return an integer exit code

