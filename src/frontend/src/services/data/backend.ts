const BASE_BACKEND_URL: string = "http://localhost:5275";

export async function PostAsync<T>(url: string, body: object): Promise<T>{
    const finalUrl = `${BASE_BACKEND_URL}/${url}`;
    const request = await fetch(finalUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    });
    
    return await request.json() as T;
} 