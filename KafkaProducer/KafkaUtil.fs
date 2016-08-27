module KafkaUtil
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

let createKafkaProducer serverName bufferingMS = 
    let c = new Config()
    c.["queue.buffering.max.ms"] <- bufferingMS;
    let producer = new Producer(c, serverName) 
    producer

let createKafkaTopic (producer:Producer) (topicName:string) = 
    let topic = producer.Topic(topicName)
    topic
