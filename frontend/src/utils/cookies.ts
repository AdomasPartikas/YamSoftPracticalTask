export class CookieManager {
  static set(name: string, value: string, expiresAt?: string): void {
    let cookieString = `${name}=${value}; path=/; SameSite=Strict`;
    
    if (expiresAt) {
      cookieString += `; expires=${new Date(expiresAt).toUTCString()}`;
    }
    
    if (window.location.protocol === 'https:') {
      cookieString += '; Secure';
    }
    
    document.cookie = cookieString;
  }

  static get(name: string): string | null {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    
    if (parts.length === 2) {
      const cookieValue = parts.pop()?.split(';').shift();
      return cookieValue || null;
    }
    
    return null;
  }

  static remove(name: string): void {
    document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT`;
  }

  static exists(name: string): boolean {
    return this.get(name) !== null;
  }
}