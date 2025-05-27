import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { User } from '../../types';

interface AuthState {
  user: User | null;
  token: string | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
  loginError: string | null;
  registerError: string | null;
}

const initialState: AuthState = {
  user: null,
  token: localStorage.getItem('token'),
  status: 'idle',
  error: null,
  loginError: null,
  registerError: null,
};

export const login = createAsyncThunk(
  'auth/login',
  async (credentials: { email: string; password: string }, { rejectWithValue, dispatch }) => {
    try {
      const response = await api.post<{ token: string; }>('/api/users/login', credentials);
      localStorage.setItem('token', response.data.token);
      
      dispatch(fetchCurrentUser());
      
      return response.data;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.detail || 'Login failed');
    }
  }
);

export const register = createAsyncThunk(
  'auth/register',
  async (userData: { username: string; email: string; password: string }, { rejectWithValue, dispatch }) => {
    try {
      const response = await api.post<{ token: string; }>('/api/users/register', userData);
      localStorage.setItem('token', response.data.token);
      
      dispatch(fetchCurrentUser());
      
      return response.data;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.detail || 'Registration failed');
    }
  }
);

export const registerAdmin = createAsyncThunk(
  'auth/registerAdmin',
  async (userData: { username: string; email: string; password: string }, { rejectWithValue }) => {
    try {
      const response = await api.post<{ token: string; }>('/api/users/register-admin', userData);
      return response.data;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.detail || 'Admin registration failed');
    }
  }
);

export const fetchCurrentUser = createAsyncThunk(
  'auth/fetchCurrentUser',
  async (_, { rejectWithValue, getState }) => {
    try {
      const { auth } = getState() as { auth: AuthState };
      
      if (!auth.token) {
        return rejectWithValue('No authentication token found');
      }
      
      const response = await api.get<User>('/api/users/me');
      return response.data;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.detail || 'Failed to fetch user data');
    }
  }
);

export const logout = createAsyncThunk('auth/logout', async () => {
  localStorage.removeItem('token');
});

export const deleteUser = createAsyncThunk(
  'auth/deleteUser',
  async (userId: string, { dispatch, rejectWithValue }) => {
    try {
      await api.delete(`/api/users/${userId}`);
      dispatch(logout());
      return userId;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.detail || 'Failed to delete user');
    }
  }
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Login cases
      .addCase(login.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(login.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.token = action.payload.token;
        state.error = null;
      })
      .addCase(login.rejected, (state, action) => {
        state.status = 'failed';
        state.loginError = action.payload as string || 'Login failed';
      })
      // Register cases
      .addCase(register.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(register.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.token = action.payload.token;
        state.error = null;
      })
      .addCase(register.rejected, (state, action) => {
        state.status = 'failed';
        state.registerError = action.payload as string || 'Registration failed';
      })
      // Register admin cases
      .addCase(registerAdmin.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(registerAdmin.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.token = action.payload.token;
        state.error = null;
      })
      .addCase(registerAdmin.rejected, (state, action) => {
        state.status = 'failed';
        state.registerError = action.payload as string || 'Admin registration failed';
      })
      // Fetch current user cases
      .addCase(fetchCurrentUser.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchCurrentUser.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.user = action.payload;
      })
      .addCase(fetchCurrentUser.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
        // Don't clear the token on user fetch failure - might be a temporary API issue
      })
      // Logout case
      .addCase(logout.fulfilled, (state) => {
        state.user = null;
        state.token = null;
        state.status = 'idle';
        state.error = null;
      })
      .addCase(deleteUser.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(deleteUser.fulfilled, (state) => {
        state.user = null;
        state.token = null;
        state.status = 'idle';
        state.error = null;
      })
      .addCase(deleteUser.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      });
  },
});

export default authSlice.reducer; 