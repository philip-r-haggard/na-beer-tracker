import { Entry } from "./entry"

export interface Member {
    id: number
    userName: string
    age: number
    knownAs: string
    created: string
    lastActive: string
    city: string
    country: string
    entries: Entry[]
}
