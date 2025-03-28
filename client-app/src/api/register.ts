import apiClient from "./axios";

interface RegisterRequest {
  username: string;
  password: string;
}

export const register = async (credentials: RegisterRequest): Promise<string> => {
  const response = await apiClient.post("/auth/register", credentials);
  return response.data.Token;
};
