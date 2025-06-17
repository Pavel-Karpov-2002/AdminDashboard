import type { Payment } from "./Payment";
import type { UserToken } from "./UserToken";

export interface User {
    id: number;
    login: string;
    email: string;
    tokenBalance: UserToken[];
    payments: Payment[];
}