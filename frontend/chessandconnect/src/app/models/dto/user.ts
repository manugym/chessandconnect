import { Play } from "../play";
import { Friend } from "./friend";

export interface User {
    id?: number
    userName?: string
    email?: string
    avatarImageUrl?: string
    role?: string
    banned?:boolean
    plays?: Play[]
    friends?: Friend[]
    friendRequest?: Friend[]
}