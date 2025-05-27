import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import type { PagedList, Topic } from '../../types';
import api from '../../api/axios';

interface TopicsState {
  paged: PagedList<Topic> | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: TopicsState = {
  paged: null,
  status: 'idle',
  error: null,
};

export const fetchTopics = createAsyncThunk(
  'topics/fetchTopics',
  async (params: { courseId: string; pageIndex?: number; pageSize?: number; orderBy?: string; orderDirection?: string }) => {
    const response = await api.get<PagedList<Topic>>(`api/topics/courses/${params.courseId}`, {
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

const topicsSlice = createSlice({
  name: 'topics',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchTopics.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(fetchTopics.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.paged = action.payload;
      })
      .addCase(fetchTopics.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch topics';
      });
  },
});

export default topicsSlice.reducer;