import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../api/axios';

export interface CompletedTopic {
  id: string;
  topicId: string;
  userId: string;
  completedAt: string;
}

interface CompletedTopicsState {
  completed: string[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: CompletedTopicsState = {
  completed: [],
  status: 'idle',
  error: null,
};

export const fetchCompletedTopics = createAsyncThunk(
  'completedTopics/fetchAll',
  async (ids: string[], { rejectWithValue }) => {
    try {
      
      const params = new URLSearchParams();
      ids.forEach(id => params.append('topicIds', id));
      const res = await api.get<CompletedTopic[]>('/api/topics/completed', {
        params,
      });
      return res.data.map(t => t.topicId);
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to fetch completed topics');
    }
  }
);

export const completeTopic = createAsyncThunk(
  'completedTopics/complete',
  async (topicId: string, { rejectWithValue }) => {
    try {
      const res = await api.post(`/api/topics/${topicId}/complete`);
      return res.data.topicId;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to complete topic');
    }
  }
);

const completedTopicsSlice = createSlice({
  name: 'completedTopics',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchCompletedTopics.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(fetchCompletedTopics.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.completed = action.payload;
      })
      .addCase(fetchCompletedTopics.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      })
      .addCase(completeTopic.fulfilled, (state, action) => {
        state.completed.push(action.payload);
      });
  },
});

export default completedTopicsSlice.reducer;