module MainKafkaProducer
open System
open System.Text
open KafkaUtil
open RdKafka


let makeKafkaMessage value iteration =
    let sMsg = sprintf "Hello #%d from %d" value iteration
    let sKey = sprintf "%d" value
    let msg = stringToUTF8Bytes sMsg
    let key = stringToUTF8Bytes sKey
    printfn "%s" sMsg
    (msg, key)

[<EntryPoint>]
let main argv = 
    let iterations = System.Int32.Parse argv.[0]
    use producer = createKafkaProducer "www.smokinserver.com:9092" "20"
    use topic = kafkaTopic producer "test"
    let sendToTestTopic = sendToTopicWithKey topic
    let publishMessage value iteration = (sendToTestTopic (makeKafkaMessage value iteration))
    let rnd = System.Random()

    for i in 1 .. iterations do
        let randomNumber = rnd.Next 9
        //let msg = makeKafkaMessage randomNumber i
 
        publishMessage randomNumber i |> ignore
        //msg |> sendToTestTopic |> ignore

    
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
    //let unused = Console.ReadLine()
    //printfn "%s" topic.Name

    0 // return an integer exit code

