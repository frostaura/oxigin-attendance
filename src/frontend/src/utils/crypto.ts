/**
 * Hash a string using SHA-256 algorithm.
 * This matches the backend's hashing implementation exactly.
 * @param input The string to hash
 * @returns The hashed string in hexadecimal format
 */
export async function hashString(input: string): Promise<string> {
    // Convert the input string to UTF-8 bytes
    const encoder = new TextEncoder();
    const data = encoder.encode(input);
    
    // Generate the hash using SHA-256
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    
    // Convert the hash to bytes
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    
    // Convert each byte to a two-digit hexadecimal string
    return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
}