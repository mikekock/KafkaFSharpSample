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

    static member fromJSON json =
        let (eventType, payload) = parseEventTypePayloadObjects json
        let audit = payload.["Audit"]
        let result:ItemCreated = {
            Id = payload.["Id"].ToString();
            Description = payload.["Description"].ToString();
            Department = System.Int32.Parse(payload.["Department"].ToString());
            Audit = AuditUsername.fromJSONObject audit
            }
        result

type ItemDeleted with
    static member toJSON item =
        let envelope:EventEnvelope<ItemDeleted> = {EventType = "ItemDeleted"; Payload = item}
        JsonConvert.SerializeObject(envelope)

    static member fromJSON json =
        let (eventType, payload) = parseEventTypePayloadObjects json
        let audit = payload.["Audit"]
        let result:ItemDeleted = {
            Id = payload.["Id"].ToString();
            Audit = AuditUsername.fromJSONObject audit
            }
        result

type ItemDescriptionChanged with
    static member toJSON item =
        let envelope:EventEnvelope<ItemDescriptionChanged> = {EventType = "ItemDescriptionChanged"; Payload = item}
        JsonConvert.SerializeObject(envelope)

    static member fromJSON json =
        let (eventType, payload) = parseEventTypePayloadObjects json
        let audit = payload.["Audit"]
        let result:ItemDescriptionChanged = {
            Id = payload.["Id"].ToString();
            Description = payload.["Description"].ToString();
            Audit = AuditUsername.fromJSONObject audit
            }
        result
