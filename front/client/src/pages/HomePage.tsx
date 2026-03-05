import { useEffect, useState } from "react";
  import { getAllPosts } from "../api/postApi";
  import { Link } from "react-router-dom";

  interface Post {
    id: number;
    title: string;
    slug: string;
    content: string;
    authorName: string;
    createdAt: string;
  }

  export default function HomePage() {
    const [posts, setPosts] = useState<Post[]>([]);

    useEffect(() => {
      getAllPosts().then((res) => setPosts(res.data));
    }, []);

    return (
      <div>
        <h1>Posts</h1>
        {posts.map((post) => (
          <div key={post.id}>
            <h2><Link to={`/posts/${post.slug}`}>{post.title}</Link></h2>
            <p>by {post.authorName} on {new Date(post.createdAt).toLocaleDateString()}</p>
          </div>
        ))}
      </div>
    );
  }