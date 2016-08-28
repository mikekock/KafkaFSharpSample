namespace ItemEvents
open System
open EventTypeJSON
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Newtonsoft.Json.Linq
open Audit

type ItemCreated = {Id:string; Description:string; Department:int; Audit:Audit.AuditUsername}
type ItemDeleted = {Id:string; Audit:Audit.AuditUsername}
type ItemDescriptionChanged = {Id:string; Description:string; Audit:Audit.AuditUsername}

type ItemCreated with
    static member toJSON item =
        let envelope:EventEnvelope<ItemCreated> = {EventType = "ItemCreated"; Payload = item}
        JsonConvert.SerializeObject(envelope)
        //envelope

    static member fromJSON json =
        let jo = JObject.Parse(json).["Payload"]
        let a = jo.["Audit"]
        let result:ItemCreated = {
            Id = jo.["Id"].ToString();
            Description = jo.["Description"].ToString();
            Department = System.Int32.Parse(jo.["Department"].ToString());
            Audit = { TimestampUTC = DateTime.Parse(a.["TimestampUTC"].ToString()); Username = a.["Username"].ToString();}
            }
        result
