import { Alert, Card, Col, Descriptions, Divider, Pagination, Row, Spin, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../store';
import { fetchCurrentUser } from '../store/slices/authSlice';
import { fetchProgresses, type CourseProgress } from '../store/slices/progressesSlice';
import type { CourseResponse } from '../types';
import { fetchCoursesByIds } from '../store/slices/coursesSlice';

const { Title, Paragraph } = Typography;

const Profile = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch<AppDispatch>();
  const { user, token, status } = useSelector((state: RootState) => state.auth);
  const { paged, status: progressesStatus } = useSelector((state: RootState) => state.progresses);
  const { coursesByIds, status: coursesStatus } = useSelector((state: RootState) => state.courses);

  const [courses, setCourses] = useState<Record<string, CourseResponse>>({});
  const [page, setPage] = useState(1);

  useEffect(() => {
    if (!token) {
      navigate('/login');
      return;
    }

    if (token && !user) {
      dispatch(fetchCurrentUser());
    }
  }, [token, user, navigate, dispatch]);

  useEffect(() => {
    if (user?.id) {
      dispatch(fetchProgresses({ userId: user?.id, pageIndex: page - 1, pageSize: 6 }));
    }
  }, [dispatch, user?.id, page]);

  useEffect(() => {
    if (!paged?.items?.length) return;
    const courseIds = paged.items.map((p: CourseProgress) => p.courseId);
    dispatch(fetchCoursesByIds(courseIds));
  }, [dispatch, paged?.items]);

  useEffect(() => {
    if (Array.isArray(coursesByIds)) {
      const map: Record<string, CourseResponse> = {};
      coursesByIds.forEach(course => {
        map[course.id] = course;
      });
      setCourses(map);
    }
  }, [coursesByIds]);

  if (!token) {
    return null;
  }

  if (status === 'loading' || progressesStatus === 'loading' || coursesStatus === 'loading') {
    return (
      <div style={{ textAlign: 'center', padding: '50px' }}>
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div style={{ maxWidth: '900px', margin: '0 auto', padding: '24px' }}>
      <Title level={2}>My Profile</Title>
      {user ? (
        <Card>
          <div style={{ display: 'flex', alignItems: 'center', marginBottom: '24px' }}>
            <div style={{ marginLeft: '24px' }}>
              <Title level={3}>{user.username}</Title>
            </div>
          </div>
          <Descriptions bordered column={1}>
            <Descriptions.Item label="Email">{user.email}</Descriptions.Item>
          </Descriptions>
        </Card>
      ) : (
        <Alert
          message="User information not available"
          description="There was a problem loading your profile information. Please try again later."
          type="warning"
          showIcon
        />
      )}

      <Divider style={{ margin: '32px 0 16px 0' }}>Your Courses</Divider>
      {paged?.items?.length ? (
        <>
          <Row gutter={[16, 16]}>
            {paged.items.map((progress) => {
              const course = courses[progress.courseId];
              return (
                <Col xs={24} sm={12} md={8} key={progress.id}>
                  <Card
                    style={{
                      borderRadius: 8,
                      boxShadow: '0 2px 8px #f0f1f2',
                      border: '1px solid #f0f0f0',
                      minHeight: 120,
                      display: 'flex',
                      flexDirection: 'column',
                      justifyContent: 'space-between',
                    }}
                  >
                    <div style={{ fontWeight: 500, fontSize: 16, marginBottom: 8 }}>
                      {course ? course.title : 'Loading...'}
                    </div>
                    <div style={{ marginBottom: 12 }}>
                      {progress.completedAt ? (
                        <span style={{ color: 'green', fontWeight: 500 }}>Completed</span>
                      ) : (
                        <span style={{ color: 'blue', fontWeight: 500 }}>In Progress</span>
                      )}
                    </div>
                    <div>
                      <Link to={`/courses/${progress.courseId}`}>
                        <span style={{ fontWeight: 500 }}>Go to course</span>
                      </Link>
                    </div>
                  </Card>
                </Col>
              );
            })}
          </Row>
          <div style={{ marginTop: 32, textAlign: 'center' }}>
            <Pagination
              current={paged.pageIndex + 1}
              pageSize={paged.pageSize}
              total={paged?.totalCount ?? 0}
              showSizeChanger={false}
              onChange={setPage}
            />
          </div>
        </>
      ) : (
        <Paragraph type="secondary">You have no course progresses yet.</Paragraph>
      )}
    </div>
  );
};

export default Profile;