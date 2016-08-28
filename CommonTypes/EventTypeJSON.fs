module EventTypeJSON

open System
open Newtonsoft.Json.Linq

type EventEnvelope<'A> = { EventType: String; Payload: 'A}

type EventTypePayload = { EventType: String; Payload: String}

let parseEventTypePayloadObjects json = 
    let jo = JObject.Parse(json)
    let eventType = jo.["EventType"]
    let payload = jo.["Payload"]
    (eventType, payload)

let parseEventTypePayload json = 
    let (eventType, payload) = parseEventTypePayloadObjects json
    let e:EventTypePayload = { EventType = eventType.ToString(); Payload = payload.ToString();}
    e
    