import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  id: string;
  email: string;
  role: string;
  exp: number;
  [roleProp]: string;
}

const roleProp = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

export const decodeToken = (token: string): JwtPayload | null => {
  try {
    return jwtDecode<JwtPayload>(token);
  } catch (error) {
    console.error('Failed to decode JWT token:', error);
    return null;
  }
};

export const getRoleFromToken = (token: string): string | null => {
  const decoded = decodeToken(token);
  return decoded?.[roleProp] || null;
};

export const isTokenExpired = (token: string): boolean => {
  const decoded = decodeToken(token);
  if (!decoded) return true;
  
  const currentTime = Math.floor(Date.now() / 1000);
  return decoded.exp < currentTime;
};

export const getUserIdFromToken = (token: string): string | null => {
  const decoded = decodeToken(token);
  return decoded?.id || null;
};

export const getEmailFromToken = (token: string): string | null => {
  const decoded = decodeToken(token);
  return decoded?.email || null;
}; 