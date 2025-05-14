import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Row, Col, Typography, Empty, Pagination } from 'antd';
import { fetchCourses } from '../store/slices/coursesSlice';
import type { RootState, AppDispatch } from '../store';
import CourseCard from '../components/features/courses/CourseCard';
import LoadingSpinner from '../components/common/LoadingSpinner';
import ErrorMessage from '../components/common/ErrorMessage';

const { Title } = Typography;

const Courses = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { paged, status, error } = useSelector((state: RootState) => state.courses);
  const [page, setPage] = useState(1);
  const pageSize = 9;

  useEffect(() => {
    if (status === 'idle' || page !== (paged?.pageIndex ?? 0) + 1) {
      dispatch(fetchCourses({ pageIndex: page - 1, pageSize }));
    }
  }, [status, dispatch, page, paged?.pageIndex]);

  const courses = paged?.items ?? [];

  if (status === 'loading') {
    return <LoadingSpinner />;
  }

  if (status === 'failed') {
    return <ErrorMessage message={error || 'Failed to load courses'} />;
  }

  if (!Array.isArray(courses) || courses.length === 0) {
    return <Empty description="No courses found" style={{ marginTop: 48 }} />;
  }

  return (
    <div style={{ padding: '24px 0' }}>
      <Title level={2}>Available Courses</Title>
      <Row gutter={[16, 16]}>
        {courses.map((course) => (
          <Col xs={24} sm={12} md={8} key={course.id}>
            <CourseCard {...course} />
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

export default Courses; 