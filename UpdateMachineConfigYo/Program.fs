// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Xml
open System.Xml.Linq

let loadDoc (name : string) = XDocument.Load(name)

let getElementFromDoc (name :string) (d : XDocument) =
    d.Element(XName.Get name)

let getElementFromElement (name: string) (e : XElement) =
    match e.Element(XName.Get name) with
    | null -> None
    | el -> Some el


let addElement (name : string) (e : XElement) =
    e.Add(XElement(XName.Get name))
    (getElementFromElement name e).Value

let getOrAddElement (name : string) (e : XElement) =
    match getElementFromElement name e with
    | None -> addElement name e
    | Some el -> el

let addAttribute (name, value) (e : XElement) =
    e.SetAttributeValue(XName.Get(name), value)
    e

let createElement (name: string) attributes =
    let e = XElement(XName.Get(name))
    Array.iter (fun a -> addAttribute a e |> ignore) attributes
    e

let createMaxConnElement address count =
    createElement "add" [| ("address", address)
                           ("maxconnection", count)|]

[<EntryPoint>]
let main argv = 
    let fileName = if argv.Length > 0 then argv.[0] else ""
    match fileName with
    | "" -> ()
    | _ -> 
        let doc = loadDoc fileName
        let connMgmt =
            doc
            |> getElementFromDoc "configuration"
            |> getOrAddElement "system.net"
            |> getOrAddElement "connectionManagement"

        connMgmt.Add(createMaxConnElement "*" "2")
        connMgmt.Add(createMaxConnElement "http://test.com" "144")

        printfn "%A" doc
        doc.Save(fileName)
    0 // return an integer exit code