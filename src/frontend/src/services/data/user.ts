export interface User {
    id: string;
    name: string;
    contactNr: string;
    email: string;
    //userType: UserType;
    password: string;
    //sessions?: UserSession[];
}

export interface UserSigninResponse{
    user: User;
    sessionId: string;
}

const USER_CONTROLLER_URL: string = "http://localhost:5275/User";

export async function SignInAsync(email: string, password: string): Promise<UserSigninResponse>{
    const url: string = `${USER_CONTROLLER_URL}/SignIn`;

    const request = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
    });
    const response = await request.json();

    if (!request.ok) {
        throw new Error(response.message || response.message);
    }

    return response;
} 