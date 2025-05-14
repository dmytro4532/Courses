import { Col, Empty, Pagination, Row, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import ErrorMessage from '../components/common/ErrorMessage';
import LoadingSpinner from '../components/common/LoadingSpinner';
import TopicCard from '../components/features/topics/TopicCard';
import type { AppDispatch, RootState } from '../store';
import { fetchTopics } from '../store/slices/topicsSlice';

const { Title } = Typography;

const Topics = () => {
  const { id: courseId } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const { paged, status, error } = useSelector((state: RootState) => state.topics);
  const [page, setPage] = useState(1);
  const pageSize = 9;

  useEffect(() => {
    if (courseId && (status === 'idle' || page !== (paged?.pageIndex ?? 0) + 1)) {
      dispatch(fetchTopics({ courseId, pageIndex: page - 1, pageSize }));
    }
  }, [status, dispatch, page, paged?.pageIndex, courseId]);

  const topics = paged?.items ?? [];

  if (status === 'loading') {
    return <LoadingSpinner />;
  }

  if (status === 'failed') {
    return <ErrorMessage message={error || 'Failed to load topics'} />;
  }

  if (!Array.isArray(topics) || topics.length === 0) {
    return <Empty description="No topics found" style={{ marginTop: 48 }} />;
  }

  return (
    <div style={{ padding: '24px 0' }}>
      <Title level={2}>Available Topics</Title>
      <Row gutter={[16, 16]}>
        {topics.map((topic) => (
          <Col xs={24} key={topic.id}>
            <TopicCard {...topic} />
          </Col>
        ))}
      </Row>
      <div style={{ marginTop: 32, textAlign: 'center' }}>
        <Pagination
          current={page}
          pageSize={pageSize}
          total={paged?.totalCount ?? 0}
          onChange={setPage}
          showSizeChanger={false}
        />
      </div>
    </div>
  );
};

export default Topics;