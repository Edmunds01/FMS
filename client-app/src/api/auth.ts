// src/api/auth.ts
import apiClient from './axios';

interface LoginRequest {
  username: string;
  password: string;
}

export const login = async (credentials: LoginRequest): Promise<string> => {
  const response = await apiClient.post('/auth/login', credentials);
  return response.data.Token;
};
