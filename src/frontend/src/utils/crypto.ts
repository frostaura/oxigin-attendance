/**
 * Hash a string using SHA-256 algorithm.
 * This matches the backend's hashing implementation.
 * @param input The string to hash
 * @returns The hashed string in hexadecimal format
 */
export async function hashString(input: string): Promise<string> {
    const encoder = new TextEncoder();
    const data = encoder.encode(input);
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
}