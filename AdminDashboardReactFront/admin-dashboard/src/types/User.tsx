export interface User {
    id: number;
    login: string;
    password: string;
    balance: Record<string, number>;
}