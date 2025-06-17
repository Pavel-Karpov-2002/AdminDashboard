import { Payment } from "./Payment";
import { UserToken } from "./UserToken";

export interface User {
    id: number;
    login: string;
    email: string;
    tokenBalance: UserToken[];
    payments: Payment[];
}