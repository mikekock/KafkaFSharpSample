module MainKafkaProducer
open System
open System.Text
open KafkaProducerUtil
open RdKafka
open ItemEvents

let itemCreatedKeyValue (item:ItemCreated) = 
        let k = stringToUTF8Bytes item.Id
        let j = ItemCreated.toJSON(item)
        let v = stringToUTF8Bytes j
        (v, k)

[<EntryPoint>]
let main argv = 
    let iterations = System.Int32.Parse argv.[0]
    use producer = createKafkaProducer "www.smokinserver.com:9092" "20"
    use topic = createKafkaTopic producer "test"
    let sendToTestTopic = sendToTopicWithKey topic
    let publishItemCreated (item:ItemCreated) = sendToTestTopic (itemCreatedKeyValue item)
    let rnd = System.Random()

    for i in 1 .. iterations do
        let randomNumber = rnd.Next 9
        let randomNumberString = sprintf "%d" randomNumber
        let audit:Audit.AuditUsername = {TimestampUTC = System.DateTime.UtcNow; Username = "mike"}
        let item:ItemCreated = {Id=randomNumberString; Description=sprintf "My description %s" randomNumberString; Department=2; Audit=audit}
        printfn "%d: %s %s %s %s" i item.Id item.Description (item.Audit.TimestampUTC.ToLongDateString()) (item.Audit.TimestampUTC.ToLongTimeString())
        publishItemCreated item |> ignore

    0 // return an integer exit code

