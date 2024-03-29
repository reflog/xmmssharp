#ifndef __XMMS_ASYNCNS_H__
#define __XMMS_ASYNCNS_H__

/***
  This file is part of libasyncns.
 
  libasyncns is free software; you can redistribute it and/or modify
  it under the terms of the GNU Lesser General Public License as
  published by the Free Software Foundation; either version 2 of the
  License, or (at your option) any later version.
 
  libasyncns is distributed in the hope that it will be useful, but
  WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
  General Public License for more details.
 
  You should have received a copy of the GNU Lesser General Public
  License along with libasyncns; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307
  USA.
***/

/*
 * This was modified for drop-in into xmms2 by tru 
 */

#include <sys/types.h>
#include <sys/socket.h>
#include <netdb.h>

/** 
 *
 * @defgroup asyncns
 * @{
 *
 * To use libasyncns allocate an asyncns_t object with
 * asyncns_new(). This will spawn a number of worker threads (or processes, depending on what is available) which
 * are subsequently used to process the queries the controlling
 * program issues via asyncns_getaddrinfo() and
 * asyncns_getnameinfo(). Use asyncns_free() to shut down the worker
 * threads/processes.
 *
 * Since libasyncns may fork off new processes you have to make sure that
 * your program is not irritated by spurious SIGCHLD signals.
 */

#ifdef  __cplusplus
extern "C" {
#endif

/** An opaque libasyncns session structure */
typedef struct asyncns asyncns_t;

/** An opaque libasyncns query structure */
typedef struct asyncns_query asyncns_query_t;

/** Allocate a new libasyncns session with n_proc worker processes */
asyncns_t* asyncns_new(unsigned n_proc);

/** Free a libasyncns session. This destroys all attached
 * asyncns_query_t objects automatically */
void asyncns_free(asyncns_t *asyncns);

/** Return the UNIX file descriptor to select() for readability
 * on. Use this function to integrate libasyncns with your custom main
 * loop. */
int asyncns_fd(asyncns_t *asyncns);

/** Process pending responses. If block is non-zero wait until at
 * least one response has been processed. If block is zero, process
 * all pending responses and return. */
int asyncns_wait(asyncns_t *asyncns, int block);

/** Issue a name to address query on the specified session. The
 * arguments are compatible with the ones of libc's
 * getaddrinfo(3). The function returns a new query object. When the
 * query is completed you may retrieve the results using
 * asyncns_getaddrinfo_done().*/
asyncns_query_t* asyncns_getaddrinfo(asyncns_t *asyncns, const char *node, const char *service, const struct addrinfo *hints);

/** Retrieve the results of a preceding asyncns_getaddrinfo()
 * call. Returns a addrinfo structure and a return value compatible
 * with libc's getaddrinfo(3). The query object q is destroyed by this
 * call and may not be used any further. Make sure to free the
 * returned addrinfo structure with asyncns_freeaddrinfo() and not
 * libc's freeaddrinfo(3)! If the query is not completed yet EAI_AGAIN
 * is returned.*/
int asyncns_getaddrinfo_done(asyncns_t *asyncns, asyncns_query_t* q, struct addrinfo **ret_res);

/** Issue an address to name query on the specified session. The
arguments are compatible with the ones of libc's getnameinfo(3). The
function returns a new query object. When the query is completed you
may retrieve the results using asyncns_getnameinfo_done(). Set gethost
(resp. getserv) to non-zero if you want to query the hostname
(resp. the service name). */
asyncns_query_t* asyncns_getnameinfo(asyncns_t *asyncns, const struct sockaddr *sa, socklen_t salen, int flags, int gethost, int getserv);

/** Retrieve the results of a preceding asyncns_getnameinfo)(
 * call. Returns the hostname and the service name in ret_host and
 * ret_serv. The query object q is destroyed by this call and may not
 * be used any further. If the query is not completed yet EAI_AGAIN is
 * returned. */
int asyncns_getnameinfo_done(asyncns_t *asyncns, asyncns_query_t* q, char *ret_host, size_t hostlen, char *ret_serv, size_t servlen);

/** Return the next completed query object. If no query has been completed yet, return NULL */
asyncns_query_t* asyncns_getnext(asyncns_t *asyncns);

/** Return the number of query objects (completed or not) attached to this session */
int asyncns_getnqueries(asyncns_t *asyncns);

/** Cancel a currently running query. q is is destroyed by this call
 * and may not be used any futher. */
void asyncns_cancel(asyncns_t *asyncns, asyncns_query_t* q);

/** Free the addrinfo structure as returned by
asyncns_getaddrinfo_done(). Make sure to use this functions instead of
the libc's freeaddrinfo()! */
void asyncns_freeaddrinfo(struct addrinfo *ai);

/** Returns non-zero when the query operation specified by q has been completed */
int asyncns_isdone(asyncns_t *asyncns, asyncns_query_t*q);

/** Assign some opaque userdata with a query object */
void asyncns_setuserdata(asyncns_t *asyncns, asyncns_query_t *q, void *userdata);

/** Return userdata assigned to a query object. Use
 * asyncns_setuserdata() to set this data. If no data has been set
 * prior to this call it returns NULL. */
void* asyncns_getuserdata(asyncns_t *asyncns, asyncns_query_t *q);

#ifdef  __cplusplus
}
#endif
    
#endif

/* @}
 */
