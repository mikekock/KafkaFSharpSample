namespace Audit
open System
open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Newtonsoft.Json.Linq

type AuditUsername = {TimestampUTC:System.DateTime; Username:string}

type AuditUsername with
    static member fromJSONObject (jo:JToken) =
        let result:AuditUsername = { TimestampUTC = DateTime.Parse(jo.["TimestampUTC"].ToString()); Username = jo.["Username"].ToString();}
        result
