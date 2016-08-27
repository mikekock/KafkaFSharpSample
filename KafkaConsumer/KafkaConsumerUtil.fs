module KafkaConsumerUtil
open System.Text
open System.Collections.Generic
open RdKafka

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

