import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { PagedList, Test } from '../../types';
import api from '../../api/axios';

interface TestsState {
  paged: PagedList<Test> | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: TestsState = {
  paged: null,
  status: 'idle',
  error: null,
};

export const fetchTests = createAsyncThunk(
  'tests/fetchTests',
  async (params: { pageIndex?: number; pageSize?: number; orderBy?: string; orderDirection?: string }) => {
    const response = await api.get<PagedList<Test>>('/api/tests', {
      params: {
        PageIndex: params.pageIndex ?? 0,
        PageSize: params.pageSize ?? 10,
        OrderBy: params.orderBy ?? 'Id',
        OrderDirection: params.orderDirection ?? 'ASC',
      },
    });
    return response.data;
  }
);

const testsSlice = createSlice({
  name: 'tests',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchTests.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchTests.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.paged = action.payload;
      })
      .addCase(fetchTests.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch tests';
      });
  },
});

export default testsSlice.reducer; 