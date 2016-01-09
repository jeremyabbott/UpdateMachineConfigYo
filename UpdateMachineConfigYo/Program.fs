// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System.Xml
open System.Xml.Linq

let xname name = XName.Get name

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

let createOrSetMaxConnElement address count (parent : XContainer) =
    let e = parent.Elements(XName.Get("add")) |> Seq.tryFind (fun x -> x.Attribute(xname "address").Value = address)
    match e with
    | Some el ->
        addAttribute ("address", address) el |> ignore
        addAttribute ("maxconnection", count) el |> ignore
    | None -> createMaxConnElement address count |> parent.Add

[<EntryPoint>]
let main argv = 
    let fileName = if argv.Length > 0 then argv.[0] else ""
    let sitename = if argv.Length > 1 then argv.[1] else ""
    let count = if argv.Length > 2 then argv.[2] else ""
    match fileName, sitename, count with
    | "",_,_ -> ()
    | _,"",_ -> ()
    | _,_,"" -> ()
    | f, s, c -> 
        let doc = loadDoc f
        let connMgmt =
            doc
            |> getElementFromDoc "configuration"
            |> getOrAddElement "system.net"
            |> getOrAddElement "connectionManagement"

        createOrSetMaxConnElement "*" "2" connMgmt
        createOrSetMaxConnElement s c connMgmt
        
        //printfn "%A" doc
        doc.Save(fileName)
    0 // return an integer exit code