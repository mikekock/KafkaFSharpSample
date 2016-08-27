module EventTypeJSON

open System
open Newtonsoft.Json.Linq

type EventEnvelope<'A> = { EventType: String; Payload: 'A}

type EventTypePayload = { EventType: String; Payload: String}

let parseEventTypePayload json = 
    let jo = JObject.Parse(json)
    let eventType = jo.["EventType"].ToString()
    let payload = jo.["Payload"].ToString()
    let e:EventTypePayload = { EventType = eventType; Payload = payload;}
    e
    