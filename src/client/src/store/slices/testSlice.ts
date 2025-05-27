import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { Test } from '../../types';
import api from '../../api/axios';

interface TestState {
  test: Test | null;
  loading: boolean;
  error: string | null;
}

const initialState: TestState = {
  test: null,
  loading: false,
  error: null,
};

export const fetchTestById = createAsyncThunk(
  'test/fetchById',
  async (testId: string) => {
    const response = await api.get<Test>(`/api/tests/${testId}`);
    return response.data;
  }
);

export const createTest = createAsyncThunk(
  'test/create',
  async (data: { title: string }) => {
    const response = await api.post<Test>('/api/tests', data);
    return response.data;
  }
);

export const updateTest = createAsyncThunk(
  'test/update',
  async ({ id, data }: { id: string; data: { title: string } }) => {
    const response = await api.put<Test>(`/api/tests/${id}`, { ...data, id });
    return response.data;
  }
);

export const deleteTest = createAsyncThunk(
  'test/delete',
  async (id: string) => {
    await api.delete(`/api/tests/${id}`);
    return id;
  }
);

const testSlice = createSlice({
  name: 'test',
  initialState,
  reducers: {
    clearTest: (state) => {
      state.test = null;
      state.error = null;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchTestById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchTestById.fulfilled, (state, action) => {
        state.loading = false;
        state.test = action.payload;
      })
      .addCase(fetchTestById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch test';
      })
      .addCase(createTest.fulfilled, (state, action) => {
        state.test = action.payload;
      })
      .addCase(updateTest.fulfilled, (state, action) => {
        state.test = action.payload;
      })
      .addCase(deleteTest.fulfilled, (state) => {
        state.test = null;
      });
  },
});

export const { clearTest } = testSlice.actions;
export default testSlice.reducer; 